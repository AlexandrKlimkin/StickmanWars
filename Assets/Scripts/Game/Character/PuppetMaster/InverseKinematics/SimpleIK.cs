using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleIK : MonoBehaviour
{
    public RobotJoint[] Joints;
    public Transform Target;
    public float SamplingDistance;
    public float LearningRate;
    public float DistanceThreshold;

    private float[] _Angles;

    private void Start()
    {
        _Angles = new float[Joints.Length];
        for (var i = 0; i < Joints.Length; i++)
        {
            _Angles[i] = Joints[i].transform.rotation.eulerAngles.z;
        }
    }

    private void Update()
    {
        InverseKinematics(Target.position, ref _Angles);
        for (var i = 0; i < Joints.Length; i++)
        {
            var rot = Joints[i].transform.rotation.eulerAngles;
            Joints[i].transform.localRotation = Quaternion.Euler(rot.x, rot.y, _Angles[i]);
        }
    }

    public Vector3 ForwardKinematics(float[] angles) {
        Vector3 prevPoint = Joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;
        for (int i = 1; i < Joints.Length; i++) {
            // Выполняет поворот вокруг новой оси
            rotation *= Quaternion.AngleAxis(angles[i - 1], Joints[i - 1].Axis);
            Vector3 nextPoint = prevPoint + rotation * Joints[i].StartOffset;

            prevPoint = nextPoint;
        }
        return prevPoint;
    }

    public float DistanceFromTarget(Vector3 target, float[] angles) {
        Vector3 point = ForwardKinematics(angles);
        return Vector3.Distance(point, target);
    }

    public float PartialGradient(Vector3 target, float[] angles, int i) {
        // Сохраняет угол,
        // который будет восстановлен позже
        float angle = angles[i];

        // Градиент: [F(x+SamplingDistance) - F(x)] / h
        float f_x = DistanceFromTarget(target, angles);

        angles[i] += SamplingDistance;
        float f_x_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_x_plus_d - f_x) / SamplingDistance;

        // Восстановление
        angles[i] = angle;

        return gradient;
    }

    public void InverseKinematics(Vector3 target, ref float[] angles) {
        if (DistanceFromTarget(target, angles) < DistanceThreshold)
            return;

        for (var i = Joints.Length - 1; i >= 0; i--) {
            // Градиентный спуск
            // Обновление : Solution -= LearningRate * Gradient
            var gradient = PartialGradient(target, angles, i);
            angles[i] -= LearningRate * gradient;

            // Преждевременное завершение
            if (DistanceFromTarget(target, angles) < DistanceThreshold)
                return;
        }
    }
}
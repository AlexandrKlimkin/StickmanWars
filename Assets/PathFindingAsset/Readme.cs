/* ============================================
 * WELCOME TO WAYPOINGS FOR 2D PATHFINDING v1.1
 * ============================================
 * 
 * Here is a simple set of instructions to guide you through setting up your waypoints.
 * 
 * 
 * 1. PACKAGE
 *    -------
 *    Import the package into a new project (I'm assuming its what you have already done).
 * 
 * 
 * 2. BACKGROUND
 *    ----------
 *    Add a background plane to the project just so you have an easier time layout out your waypoints.
 *    (you may use the background prefab for tutorial 
 * 
 * 
 * 3. #AGENT
 *    -----
 *    Attach the w2dp_Agent.cs script to your agent in the scene.
 *    (you may use the agent prefab for tutorial purposes)
 * 
 * 
 * 4. #WAYPOINTS
 *    ---------
 *    Add the first waypoint to the scene by selecting GameObject/W2DP/w2dp_Waypoint in the top menu.
 *    (the w2dp_WaypointManager will be automatically created if it does not already exist)
 * 
 *    NOTE: it is advised to change your scene view from perspective to orthographic view!
 * 
 * 
 * 5. ADDING WAYPOINTS
 *    ----------------
 *    - With the existing waypoint selected, simply click on "Add New Neighbour" in the inspector. 
 *    - This creates a new waypoint that is linked to the one on which the button was clicked.
 *    - The active waypoint now becomes the new waypoint.
 *    - You drag the new waypoint to a suitable position, then repeat step 6 to link up more waypoints.
 *    - This can be undone by pressing Ctrl/Cmd+Z on your keyboard.
 * 
 * 
 * 6. LINKING EXISTING WAYPOINTS
 *    --------------------------
 *    - With at least 2 existing waypoints selected, click on "Link Selected Neighbours" in the inspector.
 *    - This links up ALL the selected waypoints with one another.
 *    - Unless you wish to cross link every waypoint to one another (like a web), 
 *      you should try to limit the use of this function to 2 selected waypoints at each time.
 * 
 * 
 * 7. INSERT BETWEEN SELECTED WAYPOINTS
 *    ---------------------------------
 *    - With exactly 2 existing waypoints selected, click on "Insert In Between Selected Neighbours" in the inspector.
 *    - This creates a new waypoint in between the two selected waypoints, and links them all up automatically.
 * 
 * 
 * 8. DELETING NEIGHBOURS VIA INSPECTOR
 *    ---------------------------------
 *    - With the waypoint selected, look through the "Neighbours" section in the inspector.
 *    - On the same line as the neighbour to delete, click the "-" button.
 *    - This breaks the connection between the two waypoints.
 *    - This can be undone by pressing Ctrl/Cmd+Z on your keyboard.
 * 
 * 
 * 9. DELETING WAYPOINTS VIA EDITOR
 * 	  -----------------------------
 *    - With the waypoint(s) selected, hit the backspace/delete key on your keyboard.
 * 	  - Note that undoing this step might break some Neigbour Waypoint connection setups.
 * 
 * 
 * 10. TARGET DESTINATION DETECTION
 *     ----------------------------
 *     - With at least 1 w2dp_Agent, 2 w2dp_Waypoint and 1 w2dp_WaypointManager objects setup in the scene, 
 *       you can simply hit Play and start clicking away.
 *     - Do use the SampleAgentController as reference on how to get your agent to move.
 * 
 * 
 * 11. CUSTOMISING GIZMOS VIEW
 *     -----------------------
 *     - In Scene view, in the Gizmos dropdown, make sure that w2dp_Waypoint is checked if you wish to toggle the waypoints on/off via Inspector.
 *     - In Game view, in the Gizmos dropdown, make sure that w2dp_Waypoint is checked, and that Gizmos is active if you wish to toggle the waypoints on/off via Inspector.
 *     - In either views, with the w2dpWaypointManager object selected, you may turn on/off Neighbours Gizmos by checking/unchecking the ShowNeighbours box.
 * 
 * 
 * Should you have any queries or need any help with the setup, please send submit send me a note via the Contact Me form in www.TingleeStudios.com.
 * Thank you very much for purchasing Waypoints For 2D Pathfinding!
 * 
 * */

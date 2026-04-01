# Responsive_weapon

https://github.com/user-attachments/assets/9856305a-a42c-4b80-ac61-83be65a91667

**Weapon Sway and Recoil**
The weapon sway is driven by raw mouse input inverted against the camera movement. I used Quaternion.Lerp in LateUpdate to continuously pull the weapon rotation back to its origin, creating a smooth drag-and-catch-up effect. Aiming down sights uses a simple Vector3.Lerp to snap the weapon to the center of the screen. For the recoil, the offset is calculated by evaluating custom AnimationCurves over time in FixedUpdate. When a shot is fired, the script generates randomized positional and rotational intensity vectors based on whether the player is aiming or firing from the hip. These vectors are multiplied by the curve evaluation to kick the weapon back and interpolate it smoothly to its resting position.

**Projectile Ballistics**
Instead of hitscan raycasts, bullets are physical projectiles using continuous Rigidbody collision detection to prevent tunneling at high speeds. Forward momentum is applied via AddForce upon instantiation. Upon collision, the script calculates the impact vector, passes velocity data to the target for directional damage handling, and applies an extra physics kickback force to any hit rigidbodies to simulate physical impact before instantiating particle effects and cleaning up the object.

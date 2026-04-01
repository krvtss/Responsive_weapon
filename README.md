# Responsive_weapon

https://github.com/user-attachments/assets/9856305a-a42c-4b80-ac61-83be65a91667

**Weapon Sway & Recoil**
Sway is driven by inverted mouse input using `Quaternion.Lerp` to create the effect. Recoil uses custom `AnimationCurves` over time. When firing, randomized intensity vectors (scaled based on hip fire or aimed) are multiplied by the curve to kick the weapon and interpolate it back to rest.

**Projectile Ballistics**
Bullets are physical projectiles using continuous `Rigidbody` collision to prevent tunneling at high speeds.

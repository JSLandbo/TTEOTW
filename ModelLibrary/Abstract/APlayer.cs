using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Enums;
using System;

namespace ModelLibrary.Abstract
{
    public abstract class APlayer
    {
        private const float LegacyFramesPerSecond = 60.0f;
        private const float DragMultiplier = 1.5f;
        private const float GravityMultiplier = 2.5f;
        private const float MaximumFallSpeedMultiplier = 3.0f;
        private Vector2 _facingDirection = new(0, 0);

        public Vector2 Coordinates { get; set; } = new(0, 0);
        public Vector2 Direction
        {
            get => FacingDirection;
            set => FacingDirection = ToCardinalDirection(value);
        }

        public Vector2 MovementInput { get; set; } = new(0, 0);
        public Vector2 FacingDirection
        {
            get => _facingDirection;
            set => _facingDirection = ToCardinalDirection(value);
        }

        public float XVelocity { get; set; }
        public float YVelocity { get; set; }

        public float XOffset { get; set; }
        public float YOffset { get; set; }

        public PlayerOrientation Orientation
        {
            get
            {
                if (FacingDirection == new Vector2(0, 1)) return PlayerOrientation.Down;
                if (FacingDirection == new Vector2(-1, 0)) return PlayerOrientation.Left;
                if (FacingDirection == new Vector2(0, -1)) return PlayerOrientation.Up;
                if (FacingDirection == new Vector2(1, 0)) return PlayerOrientation.Right;
                return PlayerOrientation.Base;
            }
        }

        public Vector2 UpdateInput(KeyboardState currentState, KeyboardState previousState)
        {
            var up = IsPressed(currentState, Keys.Up, Keys.W);
            var down = IsPressed(currentState, Keys.Down, Keys.S);
            var left = IsPressed(currentState, Keys.Left, Keys.A);
            var right = IsPressed(currentState, Keys.Right, Keys.D);

            Vector2 input = new(0, 0);

            if (left)
            {
                input.X -= 1;
            }

            if (right)
            {
                input.X += 1;
            }

            if (up)
            {
                input.Y -= 1;
            }

            if (down)
            {
                input.Y += 1;
            }

            if (input != Vector2.Zero)
            {
                input.Normalize();
            }

            MovementInput = input;

            var newlyPressedFacing = GetNewlyPressedFacingDirection(currentState, previousState);

            if (newlyPressedFacing != Vector2.Zero)
            {
                FacingDirection = newlyPressedFacing;
            }
            else if (input == Vector2.Zero)
            {
                if (Math.Abs(XVelocity) > Math.Abs(YVelocity))
                {
                    FacingDirection = ToCardinalDirection(new Vector2(Math.Sign(XVelocity), 0));
                }
                else if (Math.Abs(YVelocity) > 0)
                {
                    FacingDirection = ToCardinalDirection(new Vector2(0, Math.Sign(YVelocity)));
                }
            }
            else if (IsSingleAxisInput(input))
            {
                FacingDirection = ToCardinalDirection(input);
            }
            else if (!FacingMatchesInput(input))
            {
                FacingDirection = ToCardinalDirection(new Vector2(input.X, 0));

                if (FacingDirection == Vector2.Zero)
                {
                    FacingDirection = ToCardinalDirection(new Vector2(0, input.Y));
                }
            }

            return MovementInput;
        }

        public bool Mining { get; set; } = false;
        public string Name { get; set; } = "Undefined";
        public double Cash { get; set; } = 0.0f;
        public AEngine Engine { get; set; }
        public AHull Hull { get; set; }
        public ADrill Drill { get; set; }
        public AInventory Inventory { get; set; }
        public AThruster Thruster { get; set; }
        public AFuelTank FuelTank { get; set; }
        public float Weight { get; } = 0.0f;

        public float MaximumActiveVelocity => new Vector2(XVelocity, YVelocity).Length();

        public void UpdateOffset(float deltaTime)
        {
            XOffset += XVelocity * deltaTime;
            YOffset += YVelocity * deltaTime;
        }

        public void UpdateVelocity(float deltaTime)
        {
            var xVelocity = XVelocity;
            var yVelocity = YVelocity;

            if (MovementInput.X == 0)
            {
                xVelocity = MoveTowards(xVelocity, 0.0f, Drag * deltaTime);
            }
            else
            {
                var xTarget = MovementInput.X * MaximumSpeed;
                var xChangeRate = Math.Sign(xVelocity) != Math.Sign(xTarget) && xVelocity != 0.0f
                    ? Acceleration + Drag
                    : Acceleration;

                xVelocity = MoveTowards(xVelocity, xTarget, xChangeRate * deltaTime);
            }

            if (MovementInput.Y != 0)
            {
                var yTarget = MovementInput.Y * MaximumSpeed;
                var yChangeRate = Math.Sign(yVelocity) != Math.Sign(yTarget) && yVelocity != 0.0f
                    ? Acceleration + Drag
                    : Acceleration;

                yVelocity = MoveTowards(yVelocity, yTarget, yChangeRate * deltaTime);
            }

            if (MovementInput.X != 0 && Math.Abs(xVelocity) < MinimumSpeed)
            {
                xVelocity = Math.Sign(MovementInput.X) * MinimumSpeed;
            }

            if (MovementInput.Y != 0 && Math.Abs(yVelocity) < MinimumSpeed)
            {
                yVelocity = Math.Sign(MovementInput.Y) * MinimumSpeed;
            }

            if (MovementInput.Y >= 0)
            {
                yVelocity += Gravity * deltaTime;
            }

            XVelocity = Math.Clamp(xVelocity, -MaximumSpeed, MaximumSpeed);
            YVelocity = Math.Clamp(yVelocity, -MaximumSpeed, MaximumFallSpeed);

            if (MovementInput.X == 0 && Math.Abs(XVelocity) < 1.0f)
            {
                XVelocity = 0.0f;
            }
        }

        public void ResetVelocity()
        {
            XVelocity = 0.0f;
            YVelocity = 0.0f;
        }

        public void ResetOffset()
        {
            XOffset = 0.0f;
            YOffset = 0.0f;
        }

        private float MaximumSpeed => Thruster.Speed * LegacyFramesPerSecond;
        private float MinimumSpeed => Thruster.MinimumVelocity * LegacyFramesPerSecond;
        private float Acceleration => Thruster.Acceleration * LegacyFramesPerSecond * LegacyFramesPerSecond;
        private float Drag => Acceleration * DragMultiplier;
        private float Gravity => Acceleration * GravityMultiplier;
        private float MaximumFallSpeed => MaximumSpeed * MaximumFallSpeedMultiplier;

        private static bool IsPressed(KeyboardState state, params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (state.IsKeyDown(key))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool WasJustPressed(KeyboardState currentState, KeyboardState previousState, params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (currentState.IsKeyDown(key) && !previousState.IsKeyDown(key))
                {
                    return true;
                }
            }

            return false;
        }

        private static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
        {
            var delta = target - current;
            var distance = delta.Length();

            if (distance <= maxDistanceDelta || distance == 0.0f)
            {
                return target;
            }

            return current + delta / distance * maxDistanceDelta;
        }

        private static float MoveTowards(float current, float target, float maxDelta)
        {
            if (Math.Abs(target - current) <= maxDelta)
            {
                return target;
            }

            return current + Math.Sign(target - current) * maxDelta;
        }

        private static Vector2 ToCardinalDirection(Vector2 value)
        {
            if (value == Vector2.Zero)
            {
                return Vector2.Zero;
            }

            if (Math.Abs(value.X) >= Math.Abs(value.Y) && value.X != 0)
            {
                return new Vector2(Math.Sign(value.X), 0);
            }

            if (value.Y != 0)
            {
                return new Vector2(0, Math.Sign(value.Y));
            }

            return Vector2.Zero;
        }

        private static bool IsSingleAxisInput(Vector2 input)
        {
            return input.X == 0 || input.Y == 0;
        }

        private bool FacingMatchesInput(Vector2 input)
        {
            if (FacingDirection == Vector2.Zero)
            {
                return false;
            }

            if (FacingDirection.X != 0)
            {
                return Math.Sign(FacingDirection.X) == Math.Sign(input.X) && input.X != 0;
            }

            return Math.Sign(FacingDirection.Y) == Math.Sign(input.Y) && input.Y != 0;
        }

        private static Vector2 GetNewlyPressedFacingDirection(KeyboardState currentState, KeyboardState previousState)
        {
            if (WasJustPressed(currentState, previousState, Keys.Right, Keys.D))
            {
                return new Vector2(1, 0);
            }

            if (WasJustPressed(currentState, previousState, Keys.Left, Keys.A))
            {
                return new Vector2(-1, 0);
            }

            if (WasJustPressed(currentState, previousState, Keys.Down, Keys.S))
            {
                return new Vector2(0, 1);
            }

            if (WasJustPressed(currentState, previousState, Keys.Up, Keys.W))
            {
                return new Vector2(0, -1);
            }

            return Vector2.Zero;
        }
    }
}

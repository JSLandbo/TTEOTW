using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Enums;

namespace ModelLibrary.Abstract
{
    public abstract class APlayer
    {
        private Vector2 _facingDirection = new(0, 0);

        public Vector2 Coordinates { get; set; } = new(0, 0);
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

        public void ApplyIntent(Vector2 movementInput, Vector2 facingDirection)
        {
            MovementInput = movementInput;
            var nextFacingDirection = ToCardinalDirection(facingDirection);

            if (nextFacingDirection != FacingDirection)
            {
                DrillExtended = false;
            }

            FacingDirection = nextFacingDirection;
        }

        public bool Mining { get; set; } = false;
        public bool DrillExtended { get; set; } = false;
        public string Name { get; set; } = "Undefined";
        public double Cash { get; set; } = 0.0f;
        public AEngine Engine { get; set; } = null!;
        public AHull Hull { get; set; } = null!;
        public ADrill Drill { get; set; } = null!;
        public AInventory Inventory { get; set; } = null!;
        public AThruster Thruster { get; set; } = null!;
        public AFuelTank FuelTank { get; set; } = null!;
        public float Weight { get; } = 0.0f;

        public float MaximumActiveVelocity => new Vector2(XVelocity, YVelocity).Length();

        public void ResetVelocity()
        {
            XVelocity = 0.0f;
            YVelocity = 0.0f;
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
    }
}

using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Enums;

namespace ModelLibrary.Abstract
{
    public abstract class APlayer
    {
        private Vector2 _facingDirection = new(0, 0);

        protected APlayer(AThermalPlating thermalPlating, AEngine engine, AHull hull, ADrill drill, AInventory inventory, AThruster thruster, AFuelTank fuelTank, AGrid gadgetSlots, bool hasGadgetBelt = false)
        {
            ThermalPlating = thermalPlating ?? throw new ArgumentNullException(nameof(thermalPlating));
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Hull = hull ?? throw new ArgumentNullException(nameof(hull));
            Drill = drill ?? throw new ArgumentNullException(nameof(drill));
            Inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            Thruster = thruster ?? throw new ArgumentNullException(nameof(thruster));
            FuelTank = fuelTank ?? throw new ArgumentNullException(nameof(fuelTank));
            GadgetSlots = gadgetSlots ?? throw new ArgumentNullException(nameof(gadgetSlots));
            HasGadgetBelt = hasGadgetBelt;
        }

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
            Vector2 nextFacingDirection = ToCardinalDirection(facingDirection);

            if (nextFacingDirection != Vector2.Zero && nextFacingDirection != FacingDirection)
            {
                DrillExtended = false;
            }

            FacingDirection = nextFacingDirection;
        }

        public bool Mining { get; set; } = false;
        public bool DrillExtended { get; set; } = false;
        public string Name { get; set; } = "Undefined";
        public double Cash { get; set; } = 100000000.0f;
        public AThermalPlating ThermalPlating { get; set; }
        public AEngine Engine { get; set; }
        public AHull Hull { get; set; }
        public ADrill Drill { get; set; }
        public AInventory Inventory { get; set; }
        public AThruster Thruster { get; set; }
        public AFuelTank FuelTank { get; set; }
        public AGrid GadgetSlots { get; set; }
        public bool HasGadgetBelt { get; set; }
        public float Weight => GetEquippedWeight() + Inventory.ContentsWeight;

        public void ResetVelocity()
        {
            XVelocity = 0.0f;
            YVelocity = 0.0f;
        }

        private float GetEquippedWeight()
        {
            return
                ThermalPlating.Weight +
                Engine.Weight +
                Hull.Weight +
                Drill.Weight +
                Inventory.Weight +
                Thruster.Weight +
                FuelTank.Weight;
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

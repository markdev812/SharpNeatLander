namespace SharpNeatLander
{
    class Lander
    {

        public double Altitude { get; private set; }
        public double Velocity { get; private set; }
        public double Fuel { get; private set; }
        public double Thrust { get; set; }

        private double _gravity;


        public void Init(double startingHeight, double startingFuel, double gravity)
        {
            Altitude = startingHeight;
            Fuel = startingFuel;
            _gravity = gravity;
        }
        public void Update(double deltaTime)
        {
            if (Fuel <= 0)
                Thrust = 0;
            Fuel -= Thrust;
            Velocity += (Thrust + _gravity) * deltaTime;

            if (Velocity < Program.TerminalVel)
                Velocity = Program.TerminalVel;

            Altitude += Velocity * deltaTime;

        }
    }
}

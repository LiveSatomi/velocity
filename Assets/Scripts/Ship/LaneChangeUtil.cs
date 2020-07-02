namespace Ship {
    public static class LaneChangeUtil {
        public static void StartChange(ShipDriver shipDriver) {
            shipDriver.ChangeDirection = shipDriver.InputDirection;
            shipDriver.ChangeStartPosition = shipDriver.transform.position.x;
        }

        public static void EndChange(ShipDriver shipDriver) {
            var transform = shipDriver.transform;

            var pos = transform.position;
            pos.x = shipDriver.ChangeStartPosition + shipDriver.laneWidth * shipDriver.ChangeDirection;
            transform.position = pos;

            shipDriver.ChangeDirection = 0;
        }
    }
}
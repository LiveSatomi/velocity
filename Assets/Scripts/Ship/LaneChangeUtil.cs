namespace Ship {
    public static class LaneChangeUtil {
        public static void StartChange(ShipDriver shipDriver, LaneChangeType type) {
            shipDriver.StartChange(type);
        }

        public static void EndChange(ShipDriver shipDriver) {
            shipDriver.EndChange();
        }
    }
}
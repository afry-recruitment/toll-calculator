use crate::vehicle::Vehicle;

pub struct Car;

impl Vehicle for Car {
    fn vehicle_type(&self) -> &str {
        "Car"
    }
}

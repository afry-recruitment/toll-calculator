use crate::vehicle::Vehicle;

pub struct Motorbike;

impl Vehicle for Motorbike {
    fn vehicle_type(&self) -> &str {
        "Motorbike"
    }
}

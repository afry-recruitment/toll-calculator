import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:8080/",
    timeout: 1000,
});

export const fetchVehicleTypes = () => {
    return api.get("/vehicle-types");
};

export const fetchTotalFee = (vehicleType, dates) => {
    return api.get("/fee?vehicle_type=" + vehicleType + "&dates=" + dates);
};
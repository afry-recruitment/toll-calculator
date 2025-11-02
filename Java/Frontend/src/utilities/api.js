import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:8080/",
    timeout: 1000,
});

export const fetchVehicleTypes = () => {
    return api.get("/vehicle-types");
};
import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:8080/",
    timeout: 1000,
});

export const fetchVehicleTypes = async () => {
    const response = await api.get("/vehicle-types");
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    return response.json();
};
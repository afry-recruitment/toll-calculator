import { CloseOutlined } from '@mui/icons-material';
import { CircularProgress, IconButton, Snackbar } from '@mui/material';
import { DatePicker, TimePicker } from '@mui/x-date-pickers';
import React, { act, useEffect, useState } from 'react';
import { fetchVehicleTypes } from '../../utilities/api';

function AppContent() {
    const [loadingVehicleTypes, setLoadingVehicleTypes] = useState(false);
    const [vehicleTypes, setVehicleTypes] = useState([]);
    const [day, setDay] = useState(null);
    const [times, setTimes] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        setLoadingVehicleTypes(true);
        fetchVehicleTypes()
            .then((response) => {
                act(() => {
                    setVehicleTypes(response.data);
                    setLoadingVehicleTypes(false);
                    setError("itt")
                })
            })
            .catch((err) => {
                act(() => {
                    setError("Cannot load vehicle types due to: " + err.message);
                    setLoadingVehicleTypes(false);
                })
            });
    }, []);

    console.log('vehicle types', vehicleTypes, 'day', day);

    const onAddTime = (selectedTime) => {
        console.log('itt', selectedTime)
        //setTimes(selectedTime);
    }

    return (
        <div style={{ margin: "20px" }}>
            <div
                style={{
                    textAlign: "center", fontSize: "80px",
                    backgroundColor: "orangered",
                    borderRadius: "40px", color: "white",
                    margin: "10px 0px"
                }}
            >
                Toll Fee Calculator
            </div>
            {loadingVehicleTypes && 
                <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                    <CircularProgress />
                    <div style={{ marginLeft: "10px" }}>Please, wait until the vehicle types are loading!</div>
                </div>
            }
            <div
                style={{
                    backgroundColor: "lightgoldenrodyellow",
                    borderRadius: "40px",
                    margin: "10px 0px", padding: "20px"
                }}
            >
                <div style={{ textAlign: 'center', paddingBottom: '10px', fontSize: "20px" }}>Choose a day and add one or more times!</div>
                <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-evenly' }}>
                    <DatePicker
                        label="Day selection"
                        value={day}
                        onChange={(selectedDay) => setDay(selectedDay)}
                    />
                    <div>
                        <TimePicker
                            label="Time selection"
                            onChange={onAddTime}
                        />
                    </div>
                </div>
            </div>
            {error && 
                <Snackbar
                    anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
                    open={error != null}
                    message={error}
                    action={
                        <IconButton style={{ color: "white"}} size="small" onClick={() => setError(null)}>
                            <CloseOutlined />
                        </IconButton>
                    }
                />
            }
      </div>
        
    );
}

export default AppContent;
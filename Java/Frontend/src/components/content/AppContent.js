import { CloseOutlined } from '@mui/icons-material';
import { CircularProgress, FormControl, IconButton, InputLabel, MenuItem, Select, Snackbar } from '@mui/material';
import { DatePicker, TimePicker } from '@mui/x-date-pickers';
import React, { useEffect, useState } from 'react';
import { fetchVehicleTypes } from '../../utilities/api';

function AppContent() {
    const [loadingVehicleTypes, setLoadingVehicleTypes] = useState(false);
    const [availableVehicleTypes, setAvailableVehicleTypes] = useState([]);
    const [selectedVehicleType, setSelectedVehicleType] = useState('');
    const [day, setDay] = useState(null);
    const [times, setTimes] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        setLoadingVehicleTypes(true);
        fetchVehicleTypes()
            .then((response) => {
                setAvailableVehicleTypes(response.data);
                setLoadingVehicleTypes(false);
                setError('itt')
            })
            .catch((err) => {
                setError('Cannot load vehicle types due to: ' + err.message);
                setLoadingVehicleTypes(false);
            });
    }, []);

    console.log('available vehicle types', availableVehicleTypes, 'selectedVehicleType', selectedVehicleType, 'day', day);

    const onVehicleTypeSelected = (event) => {
        setSelectedVehicleType(event.target.value);
    }

    const onAddTime = (selectedTime) => {
        console.log('itt', selectedTime)
        //setTimes(selectedTime);
    }

    return (
        <div style={{ margin: '20px' }}>
            <div
                style={{
                    textAlign: 'center', fontSize: '80px',
                    backgroundColor: 'orangered',
                    borderRadius: '40px', color: 'white',
                    margin: '10px 0px'
                }}
            >
                Toll Fee Calculator
            </div>
            {loadingVehicleTypes && 
                <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                    <CircularProgress />
                    <div style={{ marginLeft: '10px' }}>Please, wait until the vehicle types are loading!</div>
                </div>
            }
            <div
                style={{
                    backgroundColor: 'lightgoldenrodyellow',
                    borderRadius: '40px',
                    margin: '10px 0px', padding: '20px'
                }}
            >
                <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-evenly' }}>
                    <FormControl style={{ minWidth: '250px' }}>
                        <InputLabel id='vehicle-type-selection-label'>Vehicle Type Selection</InputLabel>
                        <Select
                            labelId='vehicle-type-selection-label'
                            id='vehicle-type-selection-select'
                            value={selectedVehicleType}
                            label='Vehicle Type Selection'
                            onChange={onVehicleTypeSelected}
                            autoWidth
                        >
                            <MenuItem value=''><em>None</em></MenuItem>
                            {availableVehicleTypes?.map(vehicleType => <MenuItem value={vehicleType}>{vehicleType}</MenuItem>)}
                        </Select>
                    </FormControl>
                    <DatePicker
                        label='Day Selection'
                        value={day}
                        onChange={(selectedDay) => setDay(selectedDay)}
                    />
                    <div>
                        <TimePicker
                            label='Time Selection'
                            onChange={onAddTime}
                        />
                    </div>
                </div>
            </div>
            {error && 
                <Snackbar
                    anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
                    open={error != null}
                    message={error}
                    action={
                        <IconButton style={{ color: 'white'}} size='small' onClick={() => setError(null)}>
                            <CloseOutlined />
                        </IconButton>
                    }
                />
            }
      </div>
        
    );
}

export default AppContent;
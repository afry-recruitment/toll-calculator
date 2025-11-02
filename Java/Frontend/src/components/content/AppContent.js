import { AddCircleRounded, CloseOutlined, DeleteOutlined } from '@mui/icons-material';
import { Button, CircularProgress, FormControl, IconButton, InputLabel, MenuItem, Select, Snackbar, Tooltip } from '@mui/material';
import { DatePicker, TimePicker } from '@mui/x-date-pickers';
import React, { useEffect, useState } from 'react';
import { fetchVehicleTypes } from '../../utilities/api';

function AppContent() {
    const [loadingVehicleTypes, setLoadingVehicleTypes] = useState(false);
    const [availableVehicleTypes, setAvailableVehicleTypes] = useState([]);
    const [selectedVehicleType, setSelectedVehicleType] = useState('');
    const [selectedDay, setSelectedDay] = useState(null);
    const [selectedTimes, setSelectedTimes] = useState([null]);
    const [loadingTotalFee, setLoadingTotalFee] = useState(false);
    const [totalFee, setTotalFee] = useState(null);
    const [error, setError] = useState(null);

    useEffect(() => {
        setLoadingVehicleTypes(true);
        fetchVehicleTypes()
            .then((response) => {
                setAvailableVehicleTypes(response.data);
                setLoadingVehicleTypes(false);
            })
            .catch((err) => {
                setError('Cannot load vehicle types due to: ' + err.message);
                setLoadingVehicleTypes(false);
            });
    }, []);

    console.log('available vehicle types', availableVehicleTypes, 'selectedVehicleType', selectedVehicleType, 'selectedDay', selectedDay, 'selectedTimes', selectedTimes);

    const onVehicleTypeSelected = (event) => {
        setSelectedVehicleType(event.target.value);
    }

    const onDayChanged = (selectedDay) => {
        setSelectedDay(selectedDay);
    }

    const onTimeChanged = (selectedTime, changeIndex) => {
        setSelectedTimes(selectedTimes.map((time, timeIndex) => timeIndex === changeIndex ? selectedTime : time));
    }

    const onTimeAdded = () => {
        setSelectedTimes([...selectedTimes, null]);
    }

    const onTimeRemoved = (removeIndex) => {
        setSelectedTimes(selectedTimes.filter((time, timeIndex) => timeIndex !== removeIndex));
    }

    const isTotalFeeCalculationDisabled = () => {
        return selectedVehicleType == null || selectedVehicleType === ''
            || selectedDay == null 
            || selectedTimes == null 
            || selectedTimes.length === 0 || selectedTimes.filter(time => time === null).length > 0;
    }

    const totalFeeCalculationDisabled = isTotalFeeCalculationDisabled();

    return (
        <div style={{ margin: '20px' }}>
            <div
                style={{
                    textAlign: 'center', fontSize: '80px',
                    backgroundColor: 'orangered',
                    borderRadius: '40px', color: 'white',
                    margin: '20px 0px'
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
                    margin: '20px 0px', padding: '20px'
                }}
            >
                <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-evenly' }}>
                    <div style={{ flex: 1, borderRight: '1px solid lightgrey', paddingRight: '20px' }}>
                        <div>Choose the vehicle type</div>
                        <div style={{  display: 'flex', justifyContent: 'center', padding: '20px 0px' }}>
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
                                    {availableVehicleTypes?.map(vehicleType => <MenuItem value={vehicleType}>{vehicleType}</MenuItem>)}
                                </Select>
                            </FormControl>
                        </div>
                    </div>
                    <div style={{ flex: 1, borderLeft: '1px solid lightgrey', paddingLeft: '20px' }}>
                        <div>Choose the day</div>
                        <div style={{  display: 'flex', justifyContent: 'center', padding: '20px 0px' }}>
                            <DatePicker
                                label='Day Selection'
                                value={selectedDay}
                                onChange={(value) => onDayChanged(value)}
                            />
                        </div>
                    </div>
                </div>
                <div style={{ paddingTop: '20px', borderTop: '2px solid lightgrey' }}>
                    <div style={{ display: 'flex', alignItems: 'center' }}>
                        <div style={{ flex: 1 }}>Pick one or more times</div>
                        <Tooltip title={'Add one more time'}>
                            <IconButton onClick={onTimeAdded}>
                                <AddCircleRounded />
                            </IconButton>
                        </Tooltip>
                    </div>
                    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-evenly', flexWrap: 'wrap' }}>
                        {selectedTimes.map((time, timeIndex) => 
                            <div key={timeIndex} style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', margin: '20px' }}>
                                <TimePicker
                                    label='Time Selection'
                                    value={time}
                                    onChange={(value) => onTimeChanged(value, timeIndex)}
                                />
                                {selectedTimes.length != 1 &&
                                    <Tooltip title={'Remove this time'}>
                                        <IconButton onClick={() => onTimeRemoved(timeIndex)}>
                                            <DeleteOutlined />
                                        </IconButton>
                                    </Tooltip>
                                }
                            </div>
                        )}
                    </div>
                </div>
            </div>
            <div style={{margin: '20px 0px', textAlign: 'center'}}>
                {totalFeeCalculationDisabled &&
                    <Tooltip title={"Calculation is disabled until you select a vehicle type, a day and at least one time!"}>
                        <Button style={{ minWidth: '300px', pointerEvents: 'auto' }} variant="contained" disabled={true}>Calculate Total Fee</Button>
                    </Tooltip>
                }
                {!totalFeeCalculationDisabled &&
                    <Button style={{ minWidth: '300px', color: 'black', backgroundColor: 'lightgreen' }} variant="contained">Calculate Total Fee</Button>
                }
            </div>
            {totalFee &&
                <div style={{margin: '20px 0px', display: 'flex', alignItems: 'center', justifyContent: 'center'}}>
                    <div 
                        style={{ 
                            minWidth: '300px', border: '3px solid', borderRadius: '20px', backgroundColor: 'black',
                            display: 'flex', alignItems: 'center', justifyContent: 'center'
                        }}
                    >
                        <div style={{ borderRight: '3px solid', borderRadius: '20px', padding: '20px', backgroundColor: 'white' }}>Total Fee</div>
                        <div style={{ flex: 1, textAlign: 'center', fontSize: '20px', fontWeight: 'bold', color: 'white' }}>{totalFee}</div>
                    </div>
                </div>
            }
            {loadingTotalFee &&
                <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                    <CircularProgress />
                    <div style={{ marginLeft: '10px' }}>Please, wait until the total fee is calculated!</div>
                </div>
            }
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
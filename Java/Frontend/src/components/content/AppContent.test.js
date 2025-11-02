import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { render, screen } from '@testing-library/react';
import { fetchVehicleTypes } from '../../utilities/api';
import AppContent from './AppContent';

jest.mock('../../utilities/api');

test('renders title', () => {
  fetchVehicleTypes.mockResolvedValue(["car"]);
  render(
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <AppContent />
    </LocalizationProvider>
  );
  const title = screen.getByText(/Toll Fee Calculator/i);
  expect(title).toBeInTheDocument();
});

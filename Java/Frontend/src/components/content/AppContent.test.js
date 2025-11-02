import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { render, screen } from '@testing-library/react';
import { act } from 'react';
import { fetchVehicleTypes } from '../../utilities/api';
import AppContent from './AppContent';
import 'dayjs/locale/sv';

jest.mock('../../utilities/api');

test('renders title', async () => {
  fetchVehicleTypes.mockResolvedValue(["car"]);
  await act(async () => {
    render(
      <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="sv">
        <AppContent />
      </LocalizationProvider>
    );
  })
  const title = screen.getByText(/Toll Fee Calculator/i);
  expect(title).toBeInTheDocument();
});
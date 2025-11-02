import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { render, screen, within } from '@testing-library/react';
import { act } from 'react';
import { fetchVehicleTypes } from '../../utilities/api';
import AppContent from './AppContent';
import 'dayjs/locale/sv';
import userEvent from '@testing-library/user-event';

jest.mock('../../utilities/api');

const renderWithLocalizationProvider = () => {
  return act(async () => {
    render(
      <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="sv">
        <AppContent />
      </LocalizationProvider>
    );
  });
}

test('renders title', async () => {
  // given the vehicle type list
  fetchVehicleTypes.mockResolvedValue({data: ["car"]});

  // waiting for the page to be rendered
  await renderWithLocalizationProvider();

  // then the title is visible
  expect(screen.getByText(/Toll Fee Calculator/i)).toBeInTheDocument();
});

test('selects vehicle type', async () => {
  // given the vehicle type list
  fetchVehicleTypes.mockResolvedValue({data: ["car"]});
  
  // waiting for the page to be rendered
  await renderWithLocalizationProvider();

  // given vehicle type select element
  const selectElement = await screen.findByLabelText('Vehicle Type Selection');
  expect(selectElement).toBeInTheDocument();

  // when clicking on vehicle type select element
  userEvent.click(selectElement);

  // when selecting an item
  const optionsPopupEl = await screen.findByRole("listbox", {
    name: 'Vehicle Type Selection'
  });
  userEvent.click(within(optionsPopupEl).getByText(/car/i));

  // then the selected item is visible
  expect(await screen.findByText(/car/i)).toBeInTheDocument();
});
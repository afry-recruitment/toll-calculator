import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import AppContent from './components/content/AppContent';
import 'dayjs/locale/sv';

function App() {
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="sv">
      <AppContent />
    </LocalizationProvider>
  );
}

export default App;

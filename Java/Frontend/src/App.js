import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import AppContent from './components/content/AppContent';

function App() {
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <AppContent />
    </LocalizationProvider>
  );
}

export default App;

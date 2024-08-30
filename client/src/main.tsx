import { ChakraProvider } from '@chakra-ui/react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import './index.css';
import { userStore } from './utils/store/userStore';
import { UserStoreContext } from './utils/store/UserStoreContext';

const rootElement = document.getElementById('root');

if (rootElement) {
	ReactDOM.createRoot(rootElement).render(
		// <React.StrictMode>
		<UserStoreContext.Provider value={userStore}>
			<BrowserRouter>
				<ChakraProvider>
					<App />
				</ChakraProvider>
			</BrowserRouter>
		</UserStoreContext.Provider>
		// </React.StrictMode>
	);
} else {
	console.error('Root element not found');
}

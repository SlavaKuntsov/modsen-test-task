import { ChakraProvider } from '@chakra-ui/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import './index.css';
// import { UserStoreContext } from './utils/store/UserStoreContext';

const rootElement = document.getElementById('root');

const queryClient = new QueryClient();

if (rootElement) {
	ReactDOM.createRoot(rootElement).render(
		// <React.StrictMode>
		// <UserStoreContext.Provider value={userStore}>
		<QueryClientProvider client={queryClient}>
			<BrowserRouter>
				{/* <ConfigProvider direction='ltr'> */}
				<ChakraProvider>
					<App />
				</ChakraProvider>
				{/* </ConfigProvider> */}
			</BrowserRouter>
		</QueryClientProvider>
		// </UserStoreContext.Provider>
		// </React.StrictMode>
	);
} else {
	console.error('Root element not found');
}

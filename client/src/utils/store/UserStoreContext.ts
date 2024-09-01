import { createContext, useContext } from 'react';
import { userStore } from './userStore';

// // Создаем контекст
// export const UserStoreContext = createContext<typeof userStore | null>(null);

// // Кастомный хук для использования контекста
// export const useUserStore = () => {
// 	const store = useContext(UserStoreContext);
// 	if (!store) {
// 		throw new Error(
// 			'useUserStore must be used within a UserStoreContext.Provider'
// 		);
// 	}
// 	return store;
// };

import kyCore from '../core/kyCore';

// Типизация данных пользователя
export interface User {
	email: string;
	password: string;
}

export interface AuthResult {
	accessToken: string;
	refreshToken: string;
}

export interface Event {
	id: string;
	name: string;
}

// Функция для получения списка пользователей
export const getEvents = async (): Promise<Event> => {
	try {
		const response = await kyCore.get('events').json<Event>();
		console.log('response: ', response);
		return response;
	} catch (error) {
		console.error('Failed to fetch users:', error);
		throw error;
	}
};

export const login = async (userData: User): Promise<AuthResult> => {
	try {
		const response = await kyCore
			.post('Users/Login', {
				json: userData,
				credentials: 'include',
			})
			.json<AuthResult>();

		localStorage.setItem('accessToken', response.accessToken);

		console.log('response: ', response);
		return response;
	} catch (error) {
		console.error('Failed to login:', error);
		throw error;
	}
};

export const registration = async (userData: User): Promise<AuthResult> => {
	try {
		const response = await kyCore
			.post('Users/Registration', { json: userData })
			.json<AuthResult>();

		localStorage.setItem('accessToken', response.accessToken);

		console.log('response: ', response);
		return response;
	} catch (error) {
		console.error('Failed to login:', error);
		throw error;
	}
};

// export const fetchUsers = async (): Promise<User[]> => {
// 	try {
// 		const response = await kyCore.get('users').json<User[]>();
// 		return response;
// 	} catch (error) {
// 		console.error('Failed to fetch users:', error);
// 		throw error;
// 	}
// };

// export const createUser = async (userData: Omit<User, 'id'>): Promise<User> => {
// 	try {
// 		const response = await kyCore
// 			.post('users', { json: userData })
// 			.json<User>();
// 		return response;
// 	} catch (error) {
// 		console.error('Failed to create user:', error);
// 		throw error;
// 	}
// };

// export const deleteUser = async (userId: string): Promise<void> => {
// 	try {
// 		await kyCore.delete(`users/${userId}`);
// 	} catch (error) {
// 		console.error('Failed to delete user:', error);
// 		throw error;
// 	}
// };

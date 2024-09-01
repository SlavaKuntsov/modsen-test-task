import kyCore from '../core/kyCore';
import { IAuthResult, IUser } from '../types';

export const getEvents = async (): Promise<Event> => {
	try {
		return await kyCore.get('events').json<Event>();
	} catch (error) {
		console.error('Failed to fetch users:', error);
		throw error;
	}
};

export const login = async (userData: IUser): Promise<boolean | string> => {
	try {
		const response = await kyCore.post('Users/Login', {
			json: userData,
			credentials: 'include',
		});

		if (!response.ok) {
			const errorResponse = await response.json();
			console.error('Login failed:', errorResponse);
			// return errorResponse; // Возвращаем текст ошибки
		}

		const responseData = await response.json<IAuthResult>();
		localStorage.setItem('accessToken', responseData.accessToken);

		return true;
	} catch (error) {
		console.error('Failed to login:', error);
		console.log(error);
		return 'Incorrect password';
	}
};

export const registration = async (
	userData: IUser
): Promise<boolean | string> => {
	try {
		console.log('userData reg: ', userData);

		const response = await kyCore
			.post('Users/ParticipantRegistration', { json: userData })
			.json<IAuthResult>();

		localStorage.setItem('accessToken', response.accessToken);

		return true;
	} catch (error) {
		console.error('Failed to login:', error);
		return 'User with this email already exists';
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

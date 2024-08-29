import kyCore from '../core/kyCore';
import { IAuthResult, IUser } from '../types/types';

export const getEvents = async (): Promise<Event> => {
	try {
		return await kyCore.get('events').json<Event>();
	} catch (error) {
		console.error('Failed to fetch users:', error);
		throw error;
	}
};

export const login = async (userData: IUser): Promise<boolean> => {
	try {
		const response = await kyCore
			.post('Users/Login', {
				json: userData,
				credentials: 'include',
			})
			.json<IAuthResult>();

		localStorage.setItem('accessToken', response.accessToken);

		return true;
	} catch (error) {
		console.error('Failed to login:', error);
		return false;
	}
};

export const registration = async (userData: IUser): Promise<IAuthResult> => {
	try {
		const response = await kyCore
			.post('Users/Registration', { json: userData })
			.json<IAuthResult>();

		localStorage.setItem('accessToken', response.accessToken);

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

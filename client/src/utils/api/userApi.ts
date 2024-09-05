import { HTTPError } from 'ky'; // Убедитесь, что HTTPError импортирован
import kyCore from '../core/kyCore';
import { IAuthResult, IUser } from '../types';

export const login = async (userData: IUser): Promise<boolean | string> => {
	console.log('login')
	try {
		const response = kyCore.post('Users/Login', {
			json: userData,
			credentials: 'include',
		});

		const responseData = await response.json<IAuthResult>();
		localStorage.setItem('accessToken', responseData.accessToken);

		return true;
	} catch (error: unknown) {
		if (error instanceof HTTPError && error.response) {
			const errorMessage = await error.response.text();
			console.error('Failed to reg admin:', errorMessage);
			return errorMessage;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
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
	} catch (error: unknown) {
		if (error instanceof HTTPError && error.response) {
			const errorMessage = await error.response.text();
			console.error('Failed to reg admin:', errorMessage);
			return errorMessage;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
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

import { HTTPError } from 'ky'; // Убедитесь, что HTTPError импортирован
import kyCore from '../core/kyCore';
import { IAuthResult, IUser, IUserUpdate } from '../types';
import { getAccessToken } from '../tokens';

export const login = async (userData: IUser): Promise<boolean | string> => {
	console.log('login');
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

export const updateParticipant = async (
	userData: IUserUpdate
): Promise<IUser | string> => {
	const accessToken = getAccessToken();
	try {
		console.log('userData reg: ', userData);

		const response = await kyCore
			.put('Users/Update', {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
				json: userData,
			})
			.json<IUser>();

		return response;
	} catch (error: unknown) {
		if (error instanceof HTTPError && error.response) {
			const errorMessage = await error.response.text();
			console.error('Failed to update:', errorMessage);
			return errorMessage;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
	}
};

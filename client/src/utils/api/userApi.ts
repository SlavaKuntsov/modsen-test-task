import { HTTPError } from 'ky'; // Убедитесь, что HTTPError импортирован
import kyCore from '../core/kyCore';
import { getAccessToken } from '../tokens';
import { IAuthResult, IDelete, IUser, IUserUpdate } from '../types';

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

export const deleteUser = async (id: IDelete): Promise<boolean | string> => {
	const accessToken = getAccessToken();
	try {
		const response = await kyCore
			.delete(`Users/Delete/${id.id}`, {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
			})
			.json<string>();

		return true;
	} catch (error: unknown) {
		if (error instanceof HTTPError && error.response) {
			const errorMessage = await error.response.text();
			console.error('Failed to delete:', errorMessage);
			return errorMessage;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
	}
};

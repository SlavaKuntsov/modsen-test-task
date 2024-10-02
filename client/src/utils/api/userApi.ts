import { HTTPError } from 'ky'; // Убедитесь, что HTTPError импортирован
import kyCore from '../core/kyCore';
import { getAccessToken } from '../tokens';
import {
	IAdmin,
	IAuthResult,
	IDelete,
	IError,
	IUser,
	IUserUpdate,
} from '../types';

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
			const errorText = await error.response.text();
			const errorMessage: IError = JSON.parse(errorText);
			console.error('Failed to reg admin:', errorMessage);
			return errorMessage.detail;
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
			const errorText = await error.response.text();
			const errorMessage: IError = JSON.parse(errorText);
			console.error('Failed to reg admin:', errorMessage);
			return errorMessage.detail;
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
			const errorText = await error.response.text();
			const errorMessage: IError = JSON.parse(errorText);
			console.error('Failed to update:', errorMessage);
			return errorMessage.detail;
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
			const errorText = await error.response.text();
			const errorMessage: IError = JSON.parse(errorText);
			console.error('Failed to delete:', errorMessage);
			return errorMessage.detail;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
	}
};

export const getAdmins = async (): Promise<IAdmin[]> => {
	console.log('getAdmins void');
	const accessToken = getAccessToken();
	try {
		return await kyCore
			.get('Users/GetAdmins', {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
			})
			.json<IAdmin[]>();
	} catch (error) {
		console.error('Failed to fetch admins:', error);
		throw error;
	}
};

export const activateAdmin = async (id: IDelete): Promise<boolean | string> => {
	console.log('getAdmins void');
	const accessToken = getAccessToken();
	try {
		const response = await kyCore
			.get(`Users/AdminActivation/${id.id}`, {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
			})
			.json<string>();

		return true;
	} catch (error: unknown) {
		if (error instanceof HTTPError && error.response) {
			const errorText = await error.response.text();
			const errorMessage: IError = JSON.parse(errorText);
			console.error('Failed to get:', errorMessage);
			return errorMessage.detail;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
	}
};

export const deactivateAdmin = async (
	id: IDelete
): Promise<boolean | string> => {
	console.log('getAdmins void');
	const accessToken = getAccessToken();
	try {
		const response = await kyCore
			.get(`Users/AdminDeactivation/${id.id}`, {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
			})
			.json<string>();

		return true;
	} catch (error: unknown) {
		if (error instanceof HTTPError && error.response) {
			const errorText = await error.response.text();
			const errorMessage: IError = JSON.parse(errorText);
			console.error('Failed to get:', errorMessage);
			return errorMessage.detail;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
	}
};

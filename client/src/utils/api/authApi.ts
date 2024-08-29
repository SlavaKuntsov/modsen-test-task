import Cookies from 'js-cookie';
import kyCore from '../core/kyCore';
import { IAuthResult, IUser } from '../types/types';

const getAccessToken = (): string | null => localStorage.getItem('accessToken');
const getRefreshToken = (): undefined | null | string =>
	Cookies.get('yummy-cackes');

export const checkAccessToken = async (): Promise<IUser | null> => {
	// UseGlobalNavigate();

	const accessToken = getAccessToken();
	const refreshToken = getRefreshToken();

	if (accessToken) {
		try {
			const response = await kyCore.get('Users/Authorize', {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
			});
			if (response.ok) {
				return await response.json<IUser>();
			}
		} catch (error) {
			console.error('Access token invalid, trying to refresh token...');
			console.error(error);
		}
	}

	// undefined === наличие токена
	if (refreshToken === undefined) {
		return await handleRefreshToken();
	} else {
		return null;
	}
};

export const handleRefreshToken = async (): Promise<IUser | null> => {
	try {
		const response = await kyCore.post('Users/RefreshToken', {
			credentials: 'include',
		});

		if (response.ok) {
			const { accessToken: newAccessToken }: IAuthResult =
				await response.json();

			localStorage.setItem('accessToken', newAccessToken);

			return await checkAccessToken(); // Проверяем access token снова
		}
	} catch (error) {
		console.error('Error refreshing token:', error);
	}

	return null;
};

export const unauthorize = async () => {
	try {
		const response = await kyCore.get('Users/Unauthorize', {
			credentials: 'include',
		});
		if (response.ok) {
			localStorage.removeItem('accessToken');
		}
	} catch (error) {
		console.error('Failed to login:', error);
		throw error;
	}
};

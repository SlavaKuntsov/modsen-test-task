import Cookies from 'js-cookie';
import { redirectTo } from '../../hooks/UseGlobalNavigate';
import kyCore from '../core/kyCore';
import { AuthResult, User } from './userApi';

const getAccessToken = (): string | null => localStorage.getItem('accessToken');
const getRefreshToken = (): undefined | null | string =>
	Cookies.get('yummy-cackes');

export const checkAccessToken = async () => {
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
				const userData = await response.json<User>();
				localStorage.setItem('user', JSON.stringify(userData));
				return userData; // Успешная авторизация
			}
		} catch (error) {
			console.error('Access token invalid, trying to refresh token...');
			console.error(error);
		}
	}

	// undefined === наличие токена
	if (refreshToken === undefined) {
		await handleRefreshToken();
	} else {
		// redirectTo('login');
		localStorage.setItem('user', JSON.stringify(null));
	}
	return null;
};

export const handleRefreshToken = async () => {
	try {
		const response = await kyCore.post('Users/RefreshToken', {
			credentials: 'include',
		});

		if (response.ok) {
			const { accessToken: newAccessToken }: AuthResult = await response.json();
			localStorage.setItem('accessToken', newAccessToken);

			await checkAccessToken(); // Проверяем access token снова
		} else {
			redirectTo('login');
			localStorage.setItem('user', JSON.stringify(null));
		}
	} catch (error) {
		console.error('Error refreshing token:', error);
		redirectTo('login');
		localStorage.setItem('user', JSON.stringify(null));
	}
};

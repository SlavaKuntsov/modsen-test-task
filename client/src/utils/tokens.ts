import Cookies from 'js-cookie';

export const getAccessToken = (): string | null =>
	localStorage.getItem('accessToken');
export const getRefreshToken = (): undefined | null | string =>
	Cookies.get('yummy-cackes');

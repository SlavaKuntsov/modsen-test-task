import ky from 'ky';
import { API_URL } from '../constants';

const kyCore = ky.create({
	prefixUrl: API_URL,
	headers: {
		'Content-Type': 'application/json',
	},
	//   credentials: 'include',
	timeout: 5000,
	retry: 0,
});

export default kyCore;

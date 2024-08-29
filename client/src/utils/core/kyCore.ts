import ky from 'ky';

const kyCore = ky.create({
	prefixUrl: 'https://localhost:7119',
	headers: {
		'Content-Type': 'application/json',
	},
	//   credentials: 'include',
	timeout: 5000,
	retry: 0,
});

export default kyCore;

import { NavigateFunction } from 'react-router-dom';

let navigateGlobal: NavigateFunction | null = null;

export const setNavigateGlobal = (navigate: NavigateFunction) => {
	navigateGlobal = navigate;
};

export const redirectTo = (path: string) => {
	console.log('navigateGlobal: ', navigateGlobal);
	if (navigateGlobal) {
		console.log('redirectTo ' + path);
		navigateGlobal(path);
	}
};

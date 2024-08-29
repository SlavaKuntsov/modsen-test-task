import { NavigateFunction, useNavigate } from 'react-router-dom';

let navigateGlobal: NavigateFunction | null = null;

const UseGlobalNavigate = () => {
	navigateGlobal = useNavigate();
};

export const redirectTo = (path: string) => {
	if (navigateGlobal) {
		navigateGlobal(path);
	} else {
		// window.location.href = '/session-timed-out';
	}
};

export default UseGlobalNavigate;

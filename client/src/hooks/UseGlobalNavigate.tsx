import { NavigateFunction, useNavigate } from 'react-router-dom';

let navigateGlobal: NavigateFunction | null = null;

const UseGlobalNavigate = () => {
  navigateGlobal = useNavigate();
};

export default UseGlobalNavigate;

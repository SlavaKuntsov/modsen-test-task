import Events from '../../components/Events/Events';
import Search from '../../components/Search';
import { IEventsFetch } from '../../utils/types';

export default function Participant() {
	return (
		<>
			<Search />
			<Events fetch={IEventsFetch.UserEvents}/>
		</>
	);
}

import Events from '../../components/Events/Events';
import { IEventsFetch } from '../../utils/types';
import Search from './../../components/Search';

export default function Admin() {
	document.title = 'Admin';

	return (
		<>
			<Search />
			<Events fetch={IEventsFetch.AllEvents} />
		</>
	);
}

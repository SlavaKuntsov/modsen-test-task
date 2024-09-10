import Events from '../../components/Events/Events';
import Search from '../../components/Search';
import { IEventsFetch } from '../../utils/types';

export default function Home() {
	document.title = 'Home';

	return (
		<>
			<Search />
			<Events fetch={IEventsFetch.AllEvents} />
		</>
	);
}

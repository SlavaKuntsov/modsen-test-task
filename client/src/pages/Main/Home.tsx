import Events from '../../components/Events/Events';
import Search from '../../components/Search';

export default function Home() {
	document.title = 'Home';

	return (
		<div className='flex flex-col items-center w-full h-full gap-5'>
			{/* <h1 className='text-3xl'>Hello Modsen!</h1> */}
			<Search />
			<Events />
		</div>
	);
}

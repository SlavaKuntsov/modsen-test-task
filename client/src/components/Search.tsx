import { GetProps } from 'antd';
import Input from 'antd/es/input';
import { eventStore } from '../utils/store/eventsStore';

type SearchProps = GetProps<typeof Input.Search>;

export default function Search() {
	const { setSearchingEvent } = eventStore;

	const { Search } = Input;

	const onSearch: SearchProps['onSearch'] = value => handleSearch(value);

	const handleSearch = (value: string) => {
		console.log(value);
		setSearchingEvent(value);
	};

	return (
		<Search
			size='large'
			placeholder='Поиск событий'
			// placeholder='Input search event'
			allowClear
			onSearch={onSearch}
			// onChange={data => handleSearch(data.target.value)}
			// enterButton={true}
			style={{ width: 400 }}
		/>
	);
}

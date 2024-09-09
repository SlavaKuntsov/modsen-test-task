import { useQuery, useQueryClient } from '@tanstack/react-query';
import { getEvent } from '../utils/api/eventsApi';
import { IEvent } from '../utils/types';
import { LayoutGroup } from 'framer-motion';

export function useEventItem(id: string | null) {
	const queryClient = useQueryClient();

	const { isInitialLoading, error, data, refetch } = useQuery<
		IEvent | undefined,
		Error
	>({
		queryKey: ['event', id],
		queryFn: async () => {
			if (!id || id == null) return undefined;
			console.log('useEventItem ' + id);
			return await getEvent(id);
		},
		staleTime: 1000 * 60 * 60,
		refetchOnWindowFocus: false,
		retry: false,
		enabled: Boolean(id),
	});

	const clearEventData = () => {
		queryClient.setQueryData(['event', id], undefined); // Обнуляем кеш
		console.log('clearEventData ------------------')
	};

	return {
		eventItem: data,
		isLoading: isInitialLoading,
		error: error,
		refetch,
		clearEventData, // Функция для обнуления данных
	};
}

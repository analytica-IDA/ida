import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import ReportsPage from '../pages/ReportsPage';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            retry: false,
        },
    },
});

describe('ReportsPage', () => {
    it('renders the page title', () => {
        render(
            <QueryClientProvider client={queryClient}>
                <ReportsPage />
            </QueryClientProvider>
        );
        expect(screen.getByText(/Relatórios Estratégicos/i)).toBeInTheDocument();
    });
});

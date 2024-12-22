# QuickNoteVault

## Setup

1. Clone the repository:
    ```bash
    git clone https://github.com/DubBro/QuickNoteVault.git
    cd QuickNoteVault/QuickNoteVault.client
    ```

2. Install dependencies:
    ```bash
    pnpm install
    ```

3. Create a `.env` file by copying the example file:
    ```bash
    cp .env.example .env
    ```

4. Update the `.env` file with your API URL:
    ```env
    VITE_API_URL=https://your-api-url
    ```

5. Run the development server:
    ```bash
    pnpm run dev
    ```

## Scripts

- `pnpm run dev`: Starts the development server.
- `pnpm run build`: Builds the project.
- `pnpm run lint`: Lints the project files.
- `pnpm run preview`: Previews the built project.
- `pnpm run prepare`: Prepares Husky hooks.

## Environment Variables

- `VITE_API_URL`: The base URL for the API.

## Dependencies

Refer to the `package.json` file for a complete list of dependencies and devDependencies.

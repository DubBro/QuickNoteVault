import antfu from '@antfu/eslint-config';
import pluginQuery from '@tanstack/eslint-plugin-query';

export default antfu({
  react: true,
  stylistic: {
    semi: true,
  },
  rules: {
    'unicorn/filename-case': ['error', {
      case: 'kebabCase',
      ignore: ['^.*\.md$'],
    }],
  },
}, pluginQuery.configs['flat/recommended']);

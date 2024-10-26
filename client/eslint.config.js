import antfu from '@antfu/eslint-config';

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
});

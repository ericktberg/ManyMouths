import React from 'react';
import { View, Text, Pressable, ScrollView, Alert } from 'react-native';
import { Plus, Timer, Users, ChefHat, Play, MapPin } from 'lucide-react-native';
import { LinearGradient } from 'expo-linear-gradient';

export interface Recipe {
  id: string;
  name: string;
  description: string;
  prepTime: number;
  cookTime: number;
  servings: number;
  tags: string[];
  estimatedCost: number;
  image?: string;
}


interface RecipeListProps {
  onCreateRecipe: () => void;
  onRecipeClick: (recipeId: string) => void;
  onMapIngredients: (recipeId: string) => void;
}

// Custom Button component for better reusability and handling state
const CustomButton = ({ children, onPress, className, disabled, ...props }: any) => {
  return (
    <Pressable
      onPress={onPress}
      disabled={disabled}
      className={className}
      {...props}
    >
      {({ pressed }) => (
        <View className={`${className} ${pressed ? 'opacity-70' : ''}`}>
          {children}
        </View>
      )}
    </Pressable>
  );
};

export function RecipeList({ onCreateRecipe, onRecipeClick, onMapIngredients }: RecipeListProps) {
  const handleRecipeLongPress = (recipe: Recipe) => {
    Alert.alert(
      'Recipe Options',
      `Choose an action for ${recipe.name}`,
      [
        {
          text: 'Cook Recipe',
          onPress: () => onRecipeClick(recipe.id),
        },
        {
          text: 'Map Ingredients',
          onPress: () => onMapIngredients(recipe.id),
        },
        {
          text: 'Cancel',
          style: 'cancel',
        },
      ]
    );
  };

  const getTagColor = (index: number) => {
    const colors = ['#FF6347', '#9ACD32', '#4169E1', '#8A2BE2', '#FFA500']; // Using hex codes for compatibility
    return colors[index % colors.length];
  };

  // State to manage hover behavior for each card
  const [hoveredCardId, setHoveredCardId] = React.useState<string | null>(null);

  return (
    <ScrollView contentContainerClassName="p-4" className="w-full items-center">
      <View className="flex-row items-start justify-between mb-6">
        <View>
          <Text className="text-3xl font-bold text-tomato">
            My Recipes
          </Text>
          <Text className="text-sm text-gray-500 mt-1">Delicious meals with cost tracking</Text>
        </View>
        <Pressable
          onPress={onCreateRecipe}
          className="bg-primary rounded-full w-11 h-11 items-center justify-center shadow-lg"
        >
          <Plus size={20} color="white" />
        </Pressable>
      </View>

      <View className="space-y-4">
        {sampleRecipes.map((recipe) => (
          <Pressable
            key={recipe.id}
            onPress={() => onRecipeClick(recipe.id)}
            onLongPress={() => handleRecipeLongPress(recipe)}
            className={`bg-white rounded-xl p-4 shadow-sm border border-gray-200 hover:transform hover:scale-[1.02] hover:border-orange
                hover:shadow-md`}
          >
            <View className="flex-row justify-between items-start mb-3">
              <View className="bg-green-500 px-2 py-1 rounded-full">
                <Text className="text-white text-xs font-semibold">${recipe.estimatedCost.toFixed(2)}</Text>
              </View>
              <View className="flex-row items-center gap-1 text-gray-400">
                <Users size={12} color="#9CA3AF" />
                <Text className="text-xs font-medium text-gray-400">{recipe.servings}</Text>
              </View>
            </View>

            <View className="mb-3">
              <Text className="font-bold text-lg text-gray-900 mb-1">{recipe.name}</Text>
              <Text className="text-sm text-gray-500 leading-relaxed">{recipe.description}</Text>
            </View>
              
            <View className="flex-row items-center gap-4 mb-3">
              <View className="flex-row items-center gap-1">
                <Timer size={12} color="#9CA3AF" />
                <Text className="text-xs text-gray-500">Prep {recipe.prepTime}m</Text>
              </View>
              <View className="flex-row items-center gap-1">
                <ChefHat size={12} color="#9CA3AF" />
                <Text className="text-xs text-gray-500">Cook {recipe.cookTime}m</Text>
              </View>
            </View>

            <View className="flex-row flex-wrap gap-2 mb-4">
              {recipe.tags.slice(0, 3).map((tag, index) => (
                <View
                  key={tag}
                  className="rounded-full px-2 py-1"
                  style={{
                    backgroundColor: getTagColor(index) + '1A', // Adds opacity
                    borderColor: getTagColor(index) + '4D', // Adds opacity
                    borderWidth: 1,
                  }}
                >
                  <Text style={{ color: getTagColor(index) }} className="text-xs">{tag}</Text>
                </View>
              ))}
            </View>

            <View className="flex-row gap-2">
              <LinearGradient
                colors={['#FF6347', '#FFA500']}
                start={{ x: 0, y: 0 }}
                end={{ x: 1, y: 0 }}
                className="flex-1 rounded-lg hover:shadow-md hover:opacity-90"
              >
                <Pressable
                  onPress={() => onRecipeClick(recipe.id)}
                  className={`flex-1 p-2 flex-row items-center justify-center `}
                >
                  <Play size={12} color="white" />
                  <Text className="text-sm ml-1 text-white">Cook</Text>
                </Pressable>
              </LinearGradient>
            </View>
          </Pressable>
        ))}
      </View>

      {sampleRecipes.length === 0 && (
        <View className="text-center py-12 items-center">
          <Text className="text-6xl mb-4">👨‍🍳</Text>
          <Text className="text-xl font-bold text-gray-900 mb-2">No recipes yet</Text>
          <Text className="text-sm text-gray-500 mb-6 text-center">
            Create your first recipe to start tracking costs
          </Text>
          <CustomButton onPress={onCreateRecipe} className="bg-primary px-4 py-2 rounded-lg">
            <Text className="text-white">Create Recipe</Text>
          </CustomButton>
        </View>
      )}
    </ScrollView>
  );
}
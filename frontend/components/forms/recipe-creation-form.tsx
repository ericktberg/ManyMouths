import React, { useEffect, useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  ScrollView,
  SafeAreaView,
  Alert,
} from 'react-native';

import {
  ArrowLeft,
  Plus,
  Trash2,
  BookOpen,
  ChefHat,
  StickyNote,
  Clock,
  Users,
  Timer,
  MapPin
} from 'lucide-react-native';
import DropDownPicker, { ItemType, ValueType } from 'react-native-dropdown-picker';

interface Ingredient {
  id: string;
  name: string;
  amount: string;
  unit: string;
  isMapped: boolean;
}

interface RecipeCreationProps {
  onBack: () => void;
  onSave: (recipe: any) => void;
}

export function RecipeCreation({ onBack, onSave }: RecipeCreationProps) {
  const [activeTab, setActiveTab] = useState<'info' | 'ingredients' | 'instructions'>('info');

  const [recipeInfo, setRecipeInfo] = useState({
    name: '',
    description: '',
    servings: '',
    prepTime: '',
    cookTime: '',
    category: 'dinner' as const,
    dietTags: [] as string[],
    proteinTag: '' as string,
    spiceLevel: '' as string,
    difficulty: '' as string,
    customTags: [] as string[]
  });

  const [ingredients, setIngredients] = useState<Ingredient[]>([]);
  const [newIngredient, setNewIngredient] = useState({
    name: '',
    amount: '',
    unit: ''
  });

  const [instructions, setInstructions] = useState('');
  const [notes, setNotes] = useState('');
  const [tagInput, setTagInput] = useState('');

  const [unitsOpen, setUnitsOpen] = useState(false);
  const [units, setUnits] = useState<ItemType<ValueType>[]>([]);

  useEffect(() => {
    setUnits([
      /* Dry Volume */
      { label: 'cup', value: 'cup' },
      { label: 'tbsp', value: 'tbsp' },
      { label: 'tsp', value: 'tsp' },

      /* Weight */
      { label: 'g', value: 'g' },
      { label: 'oz', value: 'oz' },
      { label: 'pounds', value: 'lb' },

      /* Wet Volume */
      { label: 'ml', value: 'ml' },

      /* Ingredient Specific */
      { label: 'cloves', value: 'cloves' }
    ])
  }, []);

  /*
  const units = [
    'cup', 'tbsp', 'tsp', 'oz', 'lb', 'g', 'kg', 'ml', 'l', 
    'whole', 'clove', 'slice', 'piece', 'can', 'package'
  ];*/

  const handleAddIngredient = () => {
    if (newIngredient.name && newIngredient.amount) {
      const ingredient: Ingredient = {
        id: Date.now().toString(),
        name: newIngredient.name,
        amount: newIngredient.amount,
        unit: newIngredient.unit,
        isMapped: false
      };

      setIngredients([...ingredients, ingredient]);
      setNewIngredient({ name: '', amount: '', unit: '' });
    }
  };

  const handleRemoveIngredient = (id: string) => {
    setIngredients(ingredients.filter(ing => ing.id !== id));
  };

  const handleAddCustomTag = () => {
    if (tagInput && !recipeInfo.customTags.includes(tagInput)) {
      setRecipeInfo({
        ...recipeInfo,
        customTags: [...recipeInfo.customTags, tagInput]
      });
      setTagInput('');
    }
  };

  const handleRemoveCustomTag = (tag: string) => {
    setRecipeInfo({
      ...recipeInfo,
      customTags: recipeInfo.customTags.filter(t => t !== tag)
    });
  };

  const toggleDietTag = (tagId: string) => {
    setRecipeInfo({
      ...recipeInfo,
      dietTags: recipeInfo.dietTags.includes(tagId)
        ? recipeInfo.dietTags.filter(t => t !== tagId)
        : [...recipeInfo.dietTags, tagId]
    });
  };

  const handleSave = () => {
    const recipe = {
      ...recipeInfo,
      tags: [],
      ingredients,
      instructions,
      notes,
      servings: parseInt(recipeInfo.servings) || 1,
      prepTime: parseInt(recipeInfo.prepTime) || 0,
      cookTime: parseInt(recipeInfo.cookTime) || 0
    };
    onSave(recipe);
  };

  const canSave = recipeInfo.name && ingredients.length > 0 && instructions;

  const renderTagButton = (item: any, isSelected: boolean, onPress: () => void) => (
    <TouchableOpacity
      key={item.id}
      className={`
        px-3 py-2 rounded-full border mr-2 mb-2 flex-row items-center
        ${isSelected
          ? `bg-${item.color}-500 border-${item.color}-500`
          : `border-${item.color}-300 hover:bg-${item.color}-50 hover:border-${item.color}-400`
        }
      `}
      onPress={onPress}
    >
      <Text className={`text-xs font-medium ${isSelected ? 'text-white' : `text-${item.color}-700`}`}>
        {item.emoji} {item.name}
      </Text>
    </TouchableOpacity>
  );

  const renderInfoTab = () => (
    <ScrollView className="flex-1 p-4 pb-20">
      {/* Basic Info Card */}
      <View className="bg-gradient-to-br from-blue-50 to-purple-50 border-2 border-blue-200 rounded-xl mb-6 shadow-sm">
        <View className="p-4 border-b border-blue-100">
          <View className="flex-row items-center">
            <BookOpen size={20} className="text-blue-600 mr-2" />
            <Text className="text-lg font-bold text-gray-800">Basic Information</Text>
          </View>
        </View>
        <View className="p-4 space-y-4">
          <View>
            <Text className="text-sm font-semibold text-gray-700 mb-1">Recipe Name</Text>
            <TextInput
              className="bg-white border-2 border-gray-200 focus:border-blue-400 rounded-lg px-3 py-2 text-base"
              placeholder="Enter recipe name"
              value={recipeInfo.name}
              onChangeText={(text) => setRecipeInfo({ ...recipeInfo, name: text })}
            />
          </View>

          <View>
            <Text className="text-sm font-semibold text-gray-700 mb-1">Description</Text>
            <TextInput
              className="bg-white border-2 border-gray-200 focus:border-blue-400 rounded-lg px-3 py-2 text-base min-h-20"
              placeholder="Brief description of the recipe"
              value={recipeInfo.description}
              onChangeText={(text) => setRecipeInfo({ ...recipeInfo, description: text })}
              multiline
              numberOfLines={3}
              textAlignVertical="top"
            />
          </View>
        </View>
      </View>

      {/* Timing & Servings Card */}
      <View className="bg-gradient-to-br from-orange-50 to-yellow-50 border-2 border-orange-200 rounded-xl mb-6 shadow-sm">
        <View className="p-4 border-b border-orange-100">
          <View className="flex-row items-center">
            <Clock size={20} className="text-orange-600 mr-2" />
            <Text className="text-lg font-bold text-gray-800">Timing & Servings</Text>
          </View>
        </View>
        <View className="p-4">
          <View className="flex-row space-x-4">
            <View className="flex-1">
              <View className="flex-row items-center mb-1">
                <Users size={16} className="text-blue-600 mr-1" />
                <Text className="text-sm font-semibold text-gray-700">Servings</Text>
              </View>
              <TextInput
                className="bg-white border-2 border-gray-200 focus:border-orange-400 rounded-lg px-3 py-2 text-base"
                placeholder="4"
                value={recipeInfo.servings}
                onChangeText={(text) => setRecipeInfo({ ...recipeInfo, servings: text })}
                keyboardType="numeric"
              />
            </View>
            <View className="flex-1">
              <View className="flex-row items-center mb-1">
                <ChefHat size={16} className="text-lime-600 mr-1" />
                <Text className="text-sm font-semibold text-gray-700">Prep (min)</Text>
              </View>
              <TextInput
                className="bg-white border-2 border-gray-200 focus:border-orange-400 rounded-lg px-3 py-2 text-base"
                placeholder="15"
                value={recipeInfo.prepTime}
                onChangeText={(text) => setRecipeInfo({ ...recipeInfo, prepTime: text })}
                keyboardType="numeric"
              />
            </View>
            <View className="flex-1">
              <View className="flex-row items-center mb-1">
                <Timer size={16} className="text-red-600 mr-1" />
                <Text className="text-sm font-semibold text-gray-700">Cook (min)</Text>
              </View>
              <TextInput
                className="bg-white border-2 border-gray-200 focus:border-orange-400 rounded-lg px-3 py-2 text-base"
                placeholder="30"
                value={recipeInfo.cookTime}
                onChangeText={(text) => setRecipeInfo({ ...recipeInfo, cookTime: text })}
                keyboardType="numeric"
              />
            </View>
          </View>
        </View>
      </View>
    </ScrollView>
  );

  const renderIngredientsTab = () => (
    <ScrollView className="flex-1 p-4 pb-20">
      {/* Add Ingredient Card */}
      <View style={{zIndex: 1000}} className="bg-gradient-to-br from-lime-50 to-green-50 border-2 border-lime-200 rounded-xl mb-6 shadow-sm">
        <View className="p-4 border-b border-lime-100">
          <View className="flex-row items-center">
            <Plus size={20} className="text-lime-600 mr-2" />
            <Text className="text-lg font-bold text-gray-800">Add Ingredient</Text>
          </View>
        </View>
        <View className="p-4 space-y-4">
          <View>
            <Text className="text-sm font-semibold text-gray-700 mb-1">Ingredient</Text>
            <TextInput
              className="bg-white border-2 border-gray-200 focus:border-lime-400 rounded-lg px-3 py-2 text-base"
              placeholder="e.g., Hoisin sauce"
              value={newIngredient.name}
              onChangeText={(text) => setNewIngredient({ ...newIngredient, name: text })}
            />
          </View>

          <View className="flex-row space-x-4"
            style={{
              zIndex: 1000, // A high z-index to make it appear on top
            }}>
            <View className="flex-1">
              <Text className="text-sm font-semibold text-gray-700 mb-1">Amount</Text>
              <TextInput
                className="bg-white border-2 border-gray-200 focus:border-lime-400 rounded-lg px-3 py-2 text-base"
                placeholder="1"
                value={newIngredient.amount}
                onChangeText={(text) => setNewIngredient({ ...newIngredient, amount: text })}
                keyboardType="numeric"
              />
            </View>


            <View className="flex-1">
              <Text className="text-sm font-semibold text-gray-700 mb-1">Unit</Text>
              <DropDownPicker
                items={units}
                value={newIngredient.unit}
                open={unitsOpen}
                setItems={setUnits}
                setValue={(value) => setNewIngredient(prevIngredient => ({
                  ...prevIngredient,
                  unit: value(prevIngredient)
                }))}
                setOpen={setUnitsOpen}
                dropDownContainerStyle={{
                  backgroundColor: "white",
                  borderColor: '#ccc',
                  borderWidth: 1,
                  borderTopWidth: 0, // Avoid double border between the button and the list
                  zIndex: 1000,
                }} />
            </View>
          </View>

          <TouchableOpacity
            className="bg-gray-500 hover:bg-gray-600 rounded-lg py-3 flex-row items-center justify-center shadow-lg"
            onPress={handleAddIngredient}
          >
            <Plus size={16} className="text-white mr-2" />
            <Text className="text-white font-semibold text-base">Add Ingredient</Text>
          </TouchableOpacity>
        </View>
      </View>

      {/* Ingredients List */}
      {
        ingredients.length > 0 && (
          <View className="bg-white border-2 border-green-200 rounded-xl shadow-sm">
            <View className="p-4">
              {ingredients.map((ingredient) => (
                <View key={ingredient.id} className="flex-row items-center justify-between p-3 bg-white/50 rounded-lg border border-green-100 mb-3">
                  <View className="flex-1">
                    <Text className="font-semibold text-gray-800">{ingredient.name}</Text>
                    <Text className="text-sm text-gray-500">
                      {ingredient.amount} {ingredient.unit}
                    </Text>
                  </View>
                  <TouchableOpacity
                    className="p-2 hover:bg-red-50 rounded-lg"
                    onPress={() => handleRemoveIngredient(ingredient.id)}
                  >
                    <Trash2 size={16} className="text-red-500" />
                  </TouchableOpacity>
                </View>
              ))}
            </View>
          </View>
        )
      }
    </ScrollView >
  );

  const renderInstructionsTab = () => (
    <ScrollView className="flex-1 p-4 pb-20">
      {/* Instructions Card */}
      <View className="bg-gradient-to-br from-orange-50 to-yellow-50 border-2 border-orange-200 rounded-xl mb-6 shadow-sm">
        <View className="p-4 border-b border-orange-100">
          <View className="flex-row items-center">
            <StickyNote size={20} className="text-orange-600 mr-2" />
            <Text className="text-lg font-bold text-gray-800">Cooking Instructions</Text>
          </View>
        </View>
        <View className="p-4">
          <TextInput
            className="bg-white border-2 border-gray-200 focus:border-orange-400 rounded-lg px-3 py-2 text-base min-h-48"
            placeholder="Enter step-by-step cooking instructions..."
            value={instructions}
            onChangeText={setInstructions}
            multiline
            textAlignVertical="top"
          />
        </View>
      </View>

      {/* Notes Card */}
      <View className="bg-gradient-to-br from-purple-50 to-pink-50 border-2 border-purple-200 rounded-xl shadow-sm">
        <View className="p-4 border-b border-purple-100">
          <View className="flex-row items-center">
            <StickyNote size={20} className="text-purple-600 mr-2" />
            <Text className="text-lg font-bold text-gray-800">Notes & Tips</Text>
          </View>
        </View>
        <View className="p-4">
          <Text className="text-sm font-semibold text-gray-700 mb-2">Additional Notes (Optional)</Text>
          <TextInput
            className="bg-white border-2 border-gray-200 focus:border-purple-400 rounded-lg px-3 py-2 text-base min-h-24"
            placeholder="Any additional notes, tips, or variations..."
            value={notes}
            onChangeText={setNotes}
            multiline
            textAlignVertical="top"
          />
        </View>
      </View>
    </ScrollView>
  );

  return (
    <SafeAreaView className="flex-1 bg-gray-50">
      {/* Header */}
      <View className="flex-row items-center justify-between px-4 py-3 bg-white border-b border-gray-200">
        <TouchableOpacity className="p-2" onPress={onBack}>
          <ArrowLeft size={20} className="text-gray-600" />
        </TouchableOpacity>
        <View className="flex-1 items-center">
          <Text className="text-2xl font-bold bg-gradient-to-r from-red-500 to-orange-500 text-red-500">
            Create Recipe
          </Text>
          <Text className="text-xs text-gray-500 mt-0.5">Build your culinary masterpiece</Text>
        </View>
        <TouchableOpacity
          className={`px-4 py-2 rounded-lg shadow-lg ${canSave
            ? 'bg-gradient-to-r from-red-500 to-orange-500 bg-red-500'
            : 'bg-gray-300'
            }`}
          onPress={handleSave}
          disabled={!canSave}
        >
          <Text className="text-white font-semibold text-sm">Save Recipe</Text>
        </TouchableOpacity>
      </View>

      {/* Tabs */}
      <View className="flex-row bg-gray-100/50 p-1">
        <TouchableOpacity
          className={`flex-1 py-4 items-center rounded-lg ${activeTab === 'info'
            ? 'bg-red-500 shadow-sm'
            : 'bg-transparent'
            }`}
          onPress={() => setActiveTab('info')}
        >
          <View className="flex-row items-center">
            <BookOpen size={16} className={activeTab === 'info' ? 'text-white' : 'text-gray-600'} />
            <Text className={`ml-2 font-medium ${activeTab === 'info'
              ? 'text-white'
              : 'text-gray-600'
              }`}>
              Info
            </Text>
          </View>
        </TouchableOpacity>

        <TouchableOpacity
          className={`flex-1 py-4 items-center rounded-lg ${activeTab === 'ingredients'
            ? 'bg-red-500 shadow-sm'
            : 'bg-transparent'
            }`}
          onPress={() => setActiveTab('ingredients')}
        >
          <View className="flex-row items-center">
            <ChefHat size={16} className={activeTab === 'ingredients' ? 'text-white' : 'text-gray-600'} />
            <Text className={`ml-2 font-medium ${activeTab === 'ingredients'
              ? 'text-white'
              : 'text-gray-600'
              }`}>
              Ingredients
            </Text>
          </View>
        </TouchableOpacity>

        <TouchableOpacity
          className={`flex-1 py-4 items-center rounded-lg ${activeTab === 'instructions'
            ? 'bg-red-500 shadow-sm'
            : 'bg-transparent'
            }`}
          onPress={() => setActiveTab('instructions')}
        >
          <View className="flex-row items-center">
            <StickyNote size={16} className={activeTab === 'instructions' ? 'text-white' : 'text-gray-600'} />
            <Text className={`ml-2 font-medium ${activeTab === 'instructions'
              ? 'text-white'
              : 'text-gray-600'
              }`}>
              Instructions
            </Text>
          </View>
        </TouchableOpacity>
      </View>

      {/* Tab Content */}
      {activeTab === 'info' && renderInfoTab()}
      {activeTab === 'ingredients' && renderIngredientsTab()}
      {activeTab === 'instructions' && renderInstructionsTab()}
    </SafeAreaView>
  );
}
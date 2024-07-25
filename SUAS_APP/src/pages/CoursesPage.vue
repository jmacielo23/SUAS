<template>
  <q-page>
    <q-dialog v-model="showEditDialog">
      <q-card>
        <q-card-section>
          <q-input label="ID" v-model="selectedCourse.id" readonly />
          <q-input label="First Name" v-model="selectedCourse.code" :rules="[val => !!val || 'This field is required']" />
          <q-input label="Last Name" v-model="selectedCourse.title" :rules="[val => !!val || 'This field is required']" />
          <q-input label="Credits" v-model="selectedCourse.credits" :rules="[val => !!val || 'This field is required']" />
        </q-card-section>
        <q-card-actions align="right">
          <q-btn label="Cancel" color="negative" @click="cancelEdit" />
          <q-btn label="Save Changes" color="positive" @click="saveChanges" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-dialog v-model="errorDialog">
      <q-card>
        <q-card-section class="text-negative">{{ errorMessage }}</q-card-section>
        <q-card-actions align="right">
          <q-btn color="negative" label="Close" @click="errorDialog = false" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-dialog v-model="confirmDialog">
      <q-card>
        <q-card-section class="text-negative">{{ confirmMessage }}</q-card-section>
        <q-card-actions align="right">
          <q-btn color="positive" label="Ok" @click="confirmDialog = false" />
        </q-card-actions>
      </q-card>
    </q-dialog><q-dialog v-model="errorDialog">
      <q-card>
        <q-card-section class="text-negative">{{ errorMessage }}</q-card-section>
        <q-card-actions align="right">
          <q-btn color="negative" label="Close" @click="errorDialog = false" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-dialog v-model="confirmDialog">
      <q-card>
        <q-card-section class="text-negative">{{ confirmMessage }}</q-card-section>
        <q-card-actions align="right">
          <q-btn color="positive" label="Ok" @click="confirmDialog = false" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <div class="q-pa-md" style="max-width: 800px">
      <h5> Create Course </h5>
      <hr>
      <q-form @submit.prevent="createCourse">
        <q-input label="ID" v-model="newCourse.id" :rules="[val => !!val || 'This field is required']" />
        <q-input label="Code" v-model="newCourse.code" :rules="[val => !!val || 'This field is required']" />
        <q-input label="Title" v-model="newCourse.title" :rules="[val => !!val || 'This field is required']" />
        <q-input label="Credits" v-model="newCourse.credits" />
        <hr>
        <q-btn label="Create Course" color="positive" type="submit" />
        <hr>
      </q-form>
    </div>
    <q-table :rows="courses" :columns="columns">
      <template v-slot:body-cell-delete="props">
        <q-td :props="props">
          <q-btn color="primary" label="Edit" @click="editCourse(props.row)" />
          <q-btn color="negative" label="Delete" @click="deleteCourse(props.row.id)" />
        </q-td>
      </template>
    </q-table>
  </q-page>
</template>
<script>
  import axios from 'axios';
  export default {
    data() {
      return {
        courses: [],
        showEditDialog: false,
        errorDialog: false,
        errorMessage: '',
        confirmDialog: false,
        confirmMessage: '',
        newCourse: {
          id: '',
          code: '',
          title: '',
          credits: ''
        },
        selectedCourse: {
          id: '',
          code: '',
          title: '',
          credits: ''
        },
        columns: [
          {
            name: 'id',
            align: 'left',
            label: 'ID',
            field: 'id',
          },
          {
            name: 'code',
            required: true,
            label: 'Code',
            align: 'left',
            field: row => row.code,
            format: val => `${val}`,
          },
          {
            name: 'title',
            required: true,
            label: 'Title',
            align: 'left',
            field: row => row.title,
            format: val => `${val}`,
          },
          {
            name: 'credits',
            required: true,
            label: 'Credits',
            align: 'left',
            field: row => row.credits,
            format: val => `${val}`,
          },
          {
            name: 'delete',
            label: 'Actions',
            align: 'center',
          }
        ],
      };
    },
    mounted() {
      this.fetchCourses();
    },
    methods: {
      async fetchCourses() {
        try {
          const response = await axios.get('https://localhost:44319/api/Courses');
          this.courses = response.data;
        }
        catch (error) {
          console.error('Error fetching courses:', error);
        }
      },
      async createCourse() {
        try {
          await axios.post('https://localhost:44319/api/Courses', this.newCourse);
          this.fetchCourses();
          this.newCourse = { id: '', code: '', title: '', credits: '' };
          this.confirmMessage = 'Record Saved!';
          this.confirmDialog = true;
        }
        catch (error) {
          console.error('Error creating course', error);
          if (error.response.data.errors == null) {
            this.errorMessage = error.response.data;
          }
          else {
            this.errorMessage = error.response.data.errors;
          }
          this.errorDialog = true;
        }
      },
      async editCourse(course) {
        this.selectedCourse = Object.assign({}, course);
        this.showEditDialog = true;
      },
      cancelEdit() {
        this.selectedCourse = {
          id: '',
          code: '',
          title: '',
          credits: ''
        };
        this.showEditDialog = false;
      },
      async saveChanges() {
        try {
          await axios.put(`https://localhost:44319/api/Courses/${this.selectedCourse.id}`, this.selectedCourse);
          this.fetchCourses();
          this.showEditDialog = false;
          this.confirmMessage = 'Record Updated!';
          this.confirmDialog = true;
        }
        catch (error) {
          console.error('Error in Updating the record', error);
          if (error.response.data.errors == null) {
            this.errorMessage = error.response.data;
          }
          else {
            this.errorMessage = error.response.data.errors;
          }
          this.errorDialog = true;
        }
      },
      async deleteCourse(id) {
        try {
          await axios.delete(`https://localhost:44319/api/Courses/${id}`);
          this.courses = this.courses.filter(course => course.id != id);
        }
        catch (error) {
          console.error('Error in Deleting the record', error);
          if (error.response.data.errors == null) {
            this.errorMessage = error.response.data;
          }
          else {
            this.errorMessage = error.response.data.errors;
          }
          this.errorDialog = true;
        }
      }
    }
  };
</script>
<style scoped>
</style>

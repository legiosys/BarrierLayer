<template>
  <div>
  <form >
    <div class="form-group row">
      <label for="password" class="col-sm-4">Пароль администратора</label>
      <div class="col-sm-4">
        <input id="password" v-model.trim="password" type="password" class="form-control" @blur="getBarriers">
      </div>
    </div>
    <div class="form-group row">
      <label for="barriers" class="col-sm-4">Шлагбаум</label>
      <div class="col-sm-4">
        <select id="barriers" v-model="barrier" class="form-control">
          <option v-for="barrier in barriers" :value="barrier.id">{{ barrier.address }}</option>
        </select>
      </div>
    </div>
    <div class="form-group row">
      <label for="expire" class="col-sm-4">Истекает</label>
      <div class="col-sm-4">
        <input type="datetime-local" id="expire" v-model="expire" class="form-control">
      </div>
    </div>
    <div class="form-group row">
      <div class="col-sm-5"></div>
      <button type="submit" class="btn btn-info col-sm-2" style="" @click.prevent="createGuest">Создать гостя</button>
    </div>
    <div v-if="guest" class="form-group row">
      <div class="col-sm-3"></div>
      <label class="alert alert-success col-sm-4">{{ guest?.id }}</label>
      <div class="col-sm-2">
        <a type="button" class="btn btn-success" :href="whatsAppLink"><i class="fa fa-whatsapp" style="font-size: large"></i></a>
        <button class="btn btn-secondary" @click.prevent v-clipboard:copy="directLink"><i class="fa fa-copy" style="font-size: large"></i></button>
      </div>
    </div>
    <input type="hidden" id="directLink" :value="directLink">
  </form>
  </div>
</template>

<script>
module.exports = {
  name: "admin",
  data: function (){
    return {
      password: null,
      barrier: null,
      expire: null,
      barriers: [],
      guest:null
    }
  },
  computed: {
    directLink(){
      return `http://${location.host}/ui/guest/${this.guest?.id ?? ""}`;
    },
    whatsAppLink(){
      return `https://wa.me?text=${encodeURIComponent('Ссылка для открытия шлагбаума: \t' + this.directLink)}`
    }
  },
  methods:{
    async getBarriers() {
      try{
        const result = await axios.get(`/api/Conf/GetBarriers?password=${this.password}`)
        this.barriers = result.data
      }
      catch (error){
        alert(error.response)
      }
    },
    async createGuest(){
      try{
        const result = await axios.post(`/api/Guest/Add?password=${this.password}&barrierId=${this.barrier}&expires=${this.expire}`)
        this.guest = result.data
      }
      catch (error){
        alert(error.response)
      }
    }
  }
}
</script>

<style scoped>
label{
  text-align: right;
}
</style>